package main

import (
	"fmt"
)

// 生成斐波那契数列的函数
func fibonacci(n int) []int {
	result := make([]int, n)
	result[0], result[1] = 0, 1
	for i := 2; i < n; i++ {
		result[i] = result[i-1] + result[i-2]
	}
	return result
}

func main() {
	// 生成斐波那契数列的前 10 项
	n := 10
	fibSeq := fibonacci(n)

	// 打印结果
	fmt.Printf("斐波那契数列前 %d 项: %v\n", n, fibSeq)
}
