package main

import (
	"fmt"
	"math/rand"
	"syscall"
	"unsafe"
	
	"aa/lab_01/source/levenshtain"
)


func main() {
	choose := 1
	for choose != 0 {
		printMenu()
		fmt.Scanf("%d", &choose)
		switch choose {
		case 1:
			fmt.Printf("Введите два слова:\n")
			var s1 string
			var s2 string
			fmt.Scanf("%s")
			fmt.Scanf("%s\n%s", &s1, &s2)
			fmt.Printf("Матричный алгоритм Левенштейна:\n")
			a := levenshtain.LevenshtainMatrixExpose([]rune(s1), []rune(s2))
			fmt.Printf("Ответ: %d\n\n", a)

			fmt.Printf("Матричный алгоритм Левенштейна без Окна:\n")
			qwe := levenshtain.LevenshtainMatrixNotWindow([]rune(s1), []rune(s2))
			fmt.Printf("Ответ: %d\n\n", qwe)

			fmt.Printf("Рекурсивный алгоритм Левенштейна без матрицы:\n")
			b := levenshtain.LevenshtainRecursiveMatrixless([]rune(s1), []rune(s2))
			fmt.Printf("Ответ: %d\n\n", b)
			fmt.Printf("Рекурсивный алгоритм Левенштейна с матрицей:\n")
			c := levenshtain.LevenshtainRecursiveMatrixExpose([]rune(s1), []rune(s2))
			fmt.Printf("Ответ: %d\n\n", c)
			fmt.Printf("Матричный алгоритм Дамерау-Левенштейна:\n")
			d := levenshtain.DamerauLevenshtainMatrixExpose([]rune(s1), []rune(s2))		
			fmt.Printf("Ответ: %d\n\n", d)
		case 2:
			var s1, s2 string
			totalLevenshtainMatrixTime := 0
			totalLevenshtainRecursiveMatrixlessTime := 0
			totalLevenshtainRecursiveMatrixTime := 0
			totalDamerauLevenshtainMatrixTime := 0
			symbols := "abcdefghijklmnopqrstuvwxyz0123456789"
			lenghts := [6]int{50, 150, 250, 350, 450, 550}
			for _, stringLenght := range lenghts {
				s1 = ""
				s2 = ""
				for i := 0; i < stringLenght; i++ {
					s1 += string(symbols[rand.Intn(len(symbols) - 1)])
					s2 += string(symbols[rand.Intn(len(symbols) - 1)])
				}
				tempTime := 0
				cycleTime := 10
				for k := 0; k < cycleTime; k++ {
					firstTime := getProcessorTime()
					levenshtain.LevenshtainMatrix([]rune(s1), []rune(s2))
					secondTime := getProcessorTime()
					tempTime += int(secondTime - firstTime)
				}
				totalLevenshtainMatrixTime += tempTime / cycleTime

				tempTime = 0
				for k := 0; k < cycleTime; k++ {
					firstTime := getProcessorTime()
					levenshtain.LevenshtainRecursiveMatrix([]rune(s1), []rune(s2))
					secondTime := getProcessorTime()
					tempTime += int(secondTime - firstTime)
				}
				totalLevenshtainRecursiveMatrixTime += tempTime / cycleTime

				if stringLenght < 10 {
					tempTime = 0
					for k := 0; k < cycleTime; k++ {
						firstTime := getProcessorTime()
						levenshtain.LevenshtainRecursiveMatrixless([]rune(s1), []rune(s2))
						secondTime := getProcessorTime()
						tempTime += int(secondTime - firstTime)
					}
					totalLevenshtainRecursiveMatrixlessTime += tempTime / cycleTime
				}

				tempTime = 0
				for k := 0; k < cycleTime; k++ {
					firstTime := getProcessorTime()
					levenshtain.DamerauLevenshtainMatrix([]rune(s1), []rune(s2))
					secondTime := getProcessorTime()
					tempTime += int(secondTime - firstTime)
				}
				totalDamerauLevenshtainMatrixTime += tempTime / cycleTime
				fmt.Printf("\n\n%d\n%d %d %d %d", len(s1), totalLevenshtainMatrixTime,
											  totalLevenshtainRecursiveMatrixlessTime,
											  totalLevenshtainRecursiveMatrixTime,
											  totalDamerauLevenshtainMatrixTime)
			}

		case 0:
		default:
			fmt.Printf("Такого пункта нет, попробуйте снова")
		}
		fmt.Scanf("%s")
	}
}

func printMenu() {
	fmt.Printf("\nМЕНЮ\n" +
	"1) Режим демонстрации\n" +
	"2) Режим тестирования\n" +
	"0) Выход\n\n")
}

func getProcessorTime() (int64) {
	dll, err := syscall.LoadDLL("kernel32.dll")
	if err != nil {
		fmt.Printf("Error 1")
		return 0;
	}
	qpc, err := dll.FindProc("QueryPerformanceCounter")
	if err != nil {
		fmt.Printf("Error 2")
		return 0;
	}
	var ctr int64
	ret, _, _ := qpc.Call(uintptr(unsafe.Pointer(&ctr)))
	if ret == 0 {
		return 0
	}
	return ctr
}