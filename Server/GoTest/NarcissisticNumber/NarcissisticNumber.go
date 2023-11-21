package main

import (
	"fmt"
	"math"
)

// 判断一个数是否是水仙花数
func isNarcissisticNumber(num int) bool {
	originalNum := num
	numDigits := int(math.Log10(float64(num))) + 1
	sum := 0

	for num > 0 {
		digit := num % 10
		sum += int(math.Pow(float64(digit), float64(numDigits)))
		num /= 10
	}

	return sum == originalNum
}

func main() {
	// 打印1到1000以内的所有水仙花数
	for i := 1; i <= 1000; i++ {
		if isNarcissisticNumber(i) {
			fmt.Println(i)
		}
	}
}
