package main

import "fmt"

// 求100以内的质数和
func isPrime(n int) bool {
	if n <= 1 {
		return false
	}
	for i := 2; i*i <= n; i++ {
		if n%i == 0 {
			return false
		}
	}
	return true
}

func main() {
	var a = 0
	fmt.Println("100以内的质数和为：")
	for i := 2; i <= 100; i++ {
		if isPrime(i) {
			a += i
		}
	}
	fmt.Println(a)
}
