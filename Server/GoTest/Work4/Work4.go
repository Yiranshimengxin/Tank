package main

import "fmt"

// 冒泡排序
func bubbleSort(arr []int) {
	n := len(arr)
	for i := 0; i < n-1; i++ {
		// 最后i个元素已经排好序，不需要再比较
		for j := 0; j < n-i-1; j++ {
			// 如果当前元素比下一个元素大，交换它们
			if arr[j] > arr[j+1] {
				arr[j], arr[j+1] = arr[j+1], arr[j]
			}
		}
	}
}

func main() {
	arr := []int{64, 34, 25, 12, 22, 11, 90}

	fmt.Println("未排序的数组：", arr)
	bubbleSort(arr)
	fmt.Println("排序后的数组：", arr)
}
