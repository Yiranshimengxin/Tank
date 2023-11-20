package main

import (
	"fmt"
	"time"
)

func main() {
	go StartServer()
	fmt.Println("搭建了一个服务器！")
	for {
		time.Sleep(1 * time.Second)
	}
}

func StartServer() {
	server := NewServer("127.0.0.1", 8888)
	server.Start()
}
