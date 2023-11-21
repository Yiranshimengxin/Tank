package main

import "fmt"

// 生成汉诺塔移动次数的函数
func generateHanoiMoveCount(n int) []int {
	var moveCounts []int

	var hanoi func(n int) int
	hanoi = func(n int) int {
		if n == 0 {
			return 0
		}
		return 2*hanoi(n-1) + 1
	}

	for i := 1; i <= n; i++ {
		moveCounts = append(moveCounts, hanoi(i))
	}

	return moveCounts
}

func main() {
	// 打印1个盘子到10个盘子的移动次数
	for i := 1; i <= 10; i++ {
		moveCounts := generateHanoiMoveCount(i)
		fmt.Printf("%d个盘子需要移动%d次。\n", i, moveCounts[i-1])
	}
}
