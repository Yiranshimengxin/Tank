package main

import (
	"bufio"
	"fmt"
	"image"
	"image/color"
	"image/png"
	"log"
	"math"
	"math/rand"
	"os"
	"strings"
	"time"
)

func main() {
	//Test1()

	//Test2()

	//Test3()

	//Test4()

	//Test5()

	//Test6()
}

func Test1() {
	inputReader := bufio.NewReader(os.Stdin)
	fmt.Println("Please input your name")
	input, err := inputReader.ReadString('\n')
	if err != nil {
		fmt.Printf("An error occurred:%s\n", err)
		os.Exit(1)
	} else {
		//用切片操作删除最后的\n
		name := input[:len(input)-2]
		fmt.Printf("Hello, %s! What can I do for you?\n", name)
	}

	for {
		input, err := inputReader.ReadString('\n')
		if err != nil {
			fmt.Printf("An err:%s\n", err)
			continue
		}
		input = input[:len(input)-2]
		input = strings.ToLower(input)
		switch input {
		case "":
			continue
		case "nothing", "bye":
			fmt.Println("bye")
			os.Exit(0)
		default:
			fmt.Println("sorry, what are you talk about?")
		}
	}
}

func Test2() {
	sum := 0
	for i := 1; i <= 100; i *= 2 {
		sum += i
		fmt.Println(sum)
	}
}

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

func Test3() {
	fmt.Println("100以内的质数有：")
	for i := 2; i <= 100; i++ {
		if isPrime(i) {
			fmt.Printf("%d ", i)
		}
	}
	fmt.Println()
}

// 绘制正弦曲线
func Test4() {
	const size = 300
	pic := image.NewGray(image.Rect(0, 0, size, size))
	for x := 0; x < size; x++ {
		for y := 0; y < size; y++ {
			pic.SetGray(x, y, color.Gray{255})
		}
	}

	for x := 0; x < size; x++ {
		s := float64(x) * 2 * math.Pi / size
		y := size/2 - math.Sin(s)*size/2
		pic.SetGray(x, int(y), color.Gray{0})
	}

	file, err := os.Create("Test.png")
	if err != nil {
		log.Fatal(err)
	}
	png.Encode(file, pic)
	file.Close()
}

// 绘制余弦曲线
func Test4_1() {
	const size = 300
	pic := image.NewGray(image.Rect(0, 0, size, size))
	for x := 0; x < size; x++ {
		for y := 0; y < size; y++ {
			pic.SetGray(x, y, color.Gray{255})
		}
	}

	for x := 0; x < size; x++ {
		s := float64(x) * 2 * math.Pi / size
		y := size/2 - math.Cos(s)*size/2
		pic.SetGray(x, int(y), color.Gray{0})
	}

	file, err := os.Create("Cosine.png")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	err = png.Encode(file, pic)
	if err != nil {
		log.Fatal(err)
	}
}

// 随机数游戏
func Test5() {
	rand.Seed(time.Now().UnixNano())

	// 生成一个0到100以内的随机数
	targetNumber := rand.Intn(101)

	fmt.Println("欢迎来到猜数字游戏！")
	fmt.Println("系统已生成一个0到100以内的随机数，请开始猜测。")

	var guess int
	attempts := 0

	for {
		fmt.Print("请输入你的猜测: ")
		_, err := fmt.Scanf("%d", &guess)
		if err != nil {
			fmt.Println("请输入一个有效的整数。")
			continue
		}

		attempts++

		if guess < targetNumber {
			fmt.Println("太小了！再试一次。")
		} else if guess > targetNumber {
			fmt.Println("太大了！再试一次。")
		} else {
			fmt.Printf("恭喜你，猜对了！答案是 %d，你用了 %d 次猜对。\n", targetNumber, attempts)
			break
		}
	}
}

// 九九乘法表
func Test6() {
	for i := 1; i <= 9; i++ {
		for j := 1; j <= i; j++ {
			fmt.Printf("%d * %d = %d\t", j, i, i*j)
		}
		fmt.Println()
	}
}
