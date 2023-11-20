package main

import (
	"bytes"
	"encoding/binary"
	"fmt"
	"net"
)

type User struct {
	Name    string
	Addr    string
	Channel chan string
	conn    net.Conn
	server  *Server
}

func NewUser(conn net.Conn, server *Server) *User {
	userAddr := conn.RemoteAddr().String()

	user := &User{
		Name:    userAddr,
		Addr:    userAddr,
		Channel: make(chan string),
		conn:    conn,
		server:  server,
	}
	go user.ListenMessage()
	return user
}

func (this *User) Online() {
	this.server.mapLock.Lock()
	this.server.OnlineMap[this.Name] = this
	this.server.mapLock.Unlock()
}

func (this *User) Offline() {
	this.server.mapLock.Lock()
	delete(this.server.OnlineMap, this.Name)
	this.server.mapLock.Unlock()
	this.server.Broadcast(this, "下线了！")
}

func (this *User) DoMessage(buf []byte, len int) {
	//解析消息
	msg := string(buf[:len-1])
	fmt.Println(msg)
	//广播消息
	this.server.Broadcast(this, msg)
}

func (this *User) ListenMessage() {
	for {
		msg := <-this.Channel
		fmt.Println("send msg to client", msg, ",len:", int16(len(msg)))
		bytebuf := bytes.NewBuffer([]byte{})
		binary.Write(bytebuf, binary.BigEndian, int16(len(msg)))
		binary.Write(bytebuf, binary.BigEndian, []byte(msg))
		this.conn.Write(bytebuf.Bytes())
	}
}
