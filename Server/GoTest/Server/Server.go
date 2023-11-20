package main

import (
	"fmt"
	"io"
	"net"
	"sync"
)

type Server struct {
	Ip        string
	Port      int
	OnlineMap map[string]*User
	mapLock   sync.RWMutex
	message   chan string
}

func NewServer(ip string, port int) *Server {
	server := &Server{
		Ip:        ip,
		Port:      port,
		OnlineMap: make(map[string]*User),
		message:   make(chan string),
	}
	return server
}

func (this *Server) Start() {
	listener, err := net.Listen("tcp", fmt.Sprintf("%s:%d", this.Ip, this.Port))
	if err != nil {
		fmt.Println("listen fail,err:", err)
		return
	}
	defer listener.Close()
	go this.ListenMessager()
	for {
		conn, err := listener.Accept()
		if err != nil {
			fmt.Println("accept fail,err:", err)
			continue
		}
		go this.Handler(conn)
	}
}

func (this *Server) Handler(conn net.Conn) {
	user := NewUser(conn, this)
	user.Online()
	go func() {
		buf := make([]byte, 4096)
		for {
			len, err := conn.Read(buf)
			if len == 0 {
				user.Offline()
				break
			}
			if err != nil && err != io.EOF {
				fmt.Println("conn fail,err:", err)
				return
			}
			user.DoMessage(buf, len)
		}
	}()
}

func (this *Server) Broadcast(user *User, msg string) {
	sendMsg := "[" + user.Addr + "]" + user.Name + ":" + msg
	this.message <- sendMsg
}

func (this *Server) ListenMessager() {
	for {
		msg := <-this.message
		this.mapLock.Lock()
		for _, user := range this.OnlineMap {
			user.Channel <- msg
		}
		this.mapLock.Unlock()
	}
}
