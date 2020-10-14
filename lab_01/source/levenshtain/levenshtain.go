package levenshtain

import "fmt"

// LevenshtainMatrix implement levenshtain alghoritm
// with 2-rows matrix window
func LevenshtainMatrixNotWindow(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)
	if firstLenght == 0 && secondLenght > 0 {
		answer = secondLenght
	} else if secondLenght == 0 && firstLenght > 0 {
		answer = firstLenght
	} else {
		matrix := make([][]int, len(s1) + 1)
		for i := 0; i <= len(s1); i++ {
			matrix[i] = make([]int, len(s2) + 1)
		}
		for i := 0; i < secondLenght+1; i++ {
			matrix[0][i] = i
		}
		for i := 1; i < firstLenght+1; i++ {
			matrix[i][0] = i
			for j := 1; j < secondLenght+1; j++ {

				if matrix[i - 1][j] < matrix[i][j-1] {
					matrix[i][j] = matrix[i - 1][j]
				} else {
					matrix[i][j] = matrix[i][j-1]
				}
				matrix[i][j]++
				diagonalStep := matrix[i-1][j-1]
				if s1[i-1] != s2[j-1] {
					diagonalStep++
				}
				if diagonalStep < matrix[i][j] {
					matrix[i][j] = diagonalStep
				}
			}
		}
		answer = matrix[firstLenght][secondLenght]
	}
	return answer
}

func LevenshtainMatrix(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)
	if firstLenght == 0 && secondLenght > 0 {
		answer = secondLenght
	} else if secondLenght == 0 && firstLenght > 0 {
		answer = firstLenght
	} else {
		matrixWindow := [2][]int{
			make([]int, secondLenght+1),
			make([]int, secondLenght+1),
		}
		for i := 0; i < secondLenght+1; i++ {
			matrixWindow[0][i] = i
		}
		for i := 1; i < firstLenght+1; i++ {
			matrixWindow[1][0] = i
			for j := 1; j < secondLenght+1; j++ {

				if matrixWindow[0][j] < matrixWindow[1][j-1] {
					matrixWindow[1][j] = matrixWindow[0][j]
				} else {
					matrixWindow[1][j] = matrixWindow[1][j-1]
				}
				matrixWindow[1][j]++
				diagonalStep := matrixWindow[0][j-1]
				if s1[i-1] != s2[j-1] {
					diagonalStep++
				}
				if diagonalStep < matrixWindow[1][j] {
					matrixWindow[1][j] = diagonalStep
				}
			}
			matrixWindow[0], matrixWindow[1] = matrixWindow[1], matrixWindow[0]
		}
		answer = matrixWindow[0][secondLenght]
	}
	return answer
}

// LevenshtainMatrixExpose implement levenshtain alghoritm
// with 2-rows matrix window and print process
func LevenshtainMatrixExpose(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)
	if firstLenght == 0 && secondLenght > 0 {
		answer = secondLenght
	} else if secondLenght == 0 && firstLenght > 0 {
		answer = firstLenght
	} else {
		matrixWindow := [2][]int{
			make([]int, secondLenght+1),
			make([]int, secondLenght+1),
		}
		fmt.Printf("%6s%6s", "", "0")
		matrixWindow[0][0] = 0
		for i := 1; i < secondLenght+1; i++ {
			matrixWindow[0][i] = i
			fmt.Printf("%6s", string(s2[i-1]))
		}
		fmt.Printf("\n%6s", "0")
		for i := 0; i < secondLenght+1; i++ {
			fmt.Printf("%6d", i)
		}
		fmt.Printf("\n")
		for i := 1; i < firstLenght+1; i++ {
			matrixWindow[1][0] = i
			fmt.Printf("%6s%6d", string(s1[i-1]), i)
			for j := 1; j < secondLenght+1; j++ {

				if matrixWindow[0][j] < matrixWindow[1][j-1] {
					matrixWindow[1][j] = matrixWindow[0][j]
				} else {
					matrixWindow[1][j] = matrixWindow[1][j-1]
				}
				matrixWindow[1][j]++
				diagonalStep := matrixWindow[0][j-1]
				if s1[i-1] != s2[j-1] {
					diagonalStep++
				}
				if diagonalStep < matrixWindow[1][j] {
					matrixWindow[1][j] = diagonalStep
				}
				fmt.Printf("%6d", matrixWindow[1][j])
			}
			fmt.Printf("\n")
			matrixWindow[0], matrixWindow[1] = matrixWindow[1], matrixWindow[0]
		}
		answer = matrixWindow[0][secondLenght]
	}
	return answer
}

func LevenshtainRecursiveMatrixless(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)

	if firstLenght == 0 {
		answer = secondLenght
	} else if secondLenght == 0 {
		answer = firstLenght
	} else {
		lastSymbolFirst := s1[firstLenght - 1]
		lastSymbolSecond := s2[secondLenght - 1]
		
		correction := 1
		if (lastSymbolFirst == lastSymbolSecond) {
			correction = 0
		}
		s1 = s1[:firstLenght - 1]
		answer = LevenshtainRecursiveMatrixless(s1, s2) + 1
		s2 = s2[:secondLenght - 1]
		answerMiddle := LevenshtainRecursiveMatrixless(s1, s2) + correction
		s1 = append(s1, lastSymbolFirst)
		answerSecond := LevenshtainRecursiveMatrixless(s1, s2) + 1
		s1 = append(s2, lastSymbolSecond)

		if answerMiddle < answer {
			answer = answerMiddle
		}
		if answerSecond < answer {
			answer = answerSecond
		}
	}
	return answer
}

func LevenshtainRecursiveMatrix(s1, s2 []rune) (answer int) {
	matrix := make([][]int, len(s1) + 1)
	for i := 0; i <= len(s1); i++ {
		matrix[i] = make([]int, len(s2) + 1)
		for j := 0; j <= len(s2); j++ {
			matrix[i][j] = -1
		}
	}
	answer = LevenshtainRecursiveMatrixBody(s1, s2, matrix)
	return answer
}

func LevenshtainRecursiveMatrixExpose(s1, s2 []rune) (answer int) {
	matrix := make([][]int, len(s1) + 1)
	for i := 0; i <= len(s1); i++ {
		matrix[i] = make([]int, len(s2) + 1)
		for j := 0; j <= len(s2); j++ {
			matrix[i][j] = -1
		}
	}
	firstLenght := len(s1)
	secondLenght := len(s2)
	answer = LevenshtainRecursiveMatrixBody(s1, s2, matrix)
	fmt.Printf("%6s%6s", "", "0")
		for i := 0; i < secondLenght; i++ {
			fmt.Printf("%6s", string(s2[i]))
		}
		fmt.Printf("\n%6s", "0")
		for j := 0; j <= secondLenght; j++ {
			fmt.Printf("%6d", matrix[0][j])
		}
		fmt.Printf("\n")
		for i := 1; i <= firstLenght; i++ {
			fmt.Printf("%6s", string(s1[i - 1]))
			for j := 0; j <= secondLenght; j++ {
				fmt.Printf("%6d", matrix[i][j])
			}
			fmt.Printf("\n")
		}
	return answer
}

func LevenshtainRecursiveMatrixBody(s1, s2 []rune, matrix [][]int) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)

	if firstLenght == 0 {
		answer = secondLenght
	} else if secondLenght == 0 {
		answer = firstLenght
	} else {
		lastSymbolFirst := s1[firstLenght - 1]
		lastSymbolSecond := s2[secondLenght - 1]
		
		correction := 1
		if (lastSymbolFirst == lastSymbolSecond) {
			correction = 0
		}
		s1 = s1[:firstLenght - 1]
		if (matrix[firstLenght - 1][secondLenght] == -1) {
			LevenshtainRecursiveMatrixBody(s1, s2, matrix)
		}
		answer = matrix[firstLenght - 1][secondLenght] + 1
		s2 = s2[:secondLenght - 1]
		if (matrix[firstLenght - 1][secondLenght - 1] == -1) {
			LevenshtainRecursiveMatrixBody(s1, s2, matrix)
		}
		answerMiddle := matrix[firstLenght - 1][secondLenght - 1] + correction
		s1 = append(s1, lastSymbolFirst)
		if (matrix[firstLenght][secondLenght - 1] == -1) {
			LevenshtainRecursiveMatrixBody(s1, s2, matrix)
		}
		answerSecond := matrix[firstLenght][secondLenght - 1] + 1
		s1 = append(s2, lastSymbolSecond)

		if answerMiddle < answer {
			answer = answerMiddle
		}
		if answerSecond < answer {
			answer = answerSecond
		}
	}
	matrix[firstLenght][secondLenght] = answer
	return answer
}

func DamerauLevenshtainMatrix(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)
	if firstLenght == 0 && secondLenght > 0 {
		answer = secondLenght
	} else if secondLenght == 0 && firstLenght > 0 {
		answer = firstLenght
	} else {
		matrix := make([][]int, len(s1) + 1)
		for i := 0; i <= len(s1); i++ {
			matrix[i] = make([]int, len(s2) + 1)
		}

		for i := 0; i < secondLenght+1; i++ {
			matrix[0][i] = i
		}
		for i := 1; i < firstLenght+1; i++ {
			matrix[i][0] = i
			for j := 1; j < secondLenght+1; j++ {
				if matrix[i-1][j] < matrix[i-1][j-1] {
					matrix[i][j] = matrix[i-1][j]
				} else {
					matrix[i][j] = matrix[i][j-1]
				}
				matrix[i][j]++
				diagonalStep := matrix[i-1][j-1]
				if s1[i-1] != s2[j-1] {
					diagonalStep++
				}
				if diagonalStep < matrix[i][j] {
					matrix[i][j] = diagonalStep
				}
				
				if (i > 1 && j > 1 && s1[i - 1] == s2[j - 2] && s1[i - 2] == s2[j - 1]) {
					if (matrix[i - 2][j - 2] + 1 < matrix[i][j]) {
						matrix[i][j] = matrix[i - 2][j - 2] + 1
					}
				}
			}
		}
		answer = matrix[firstLenght][secondLenght]
	}
	return answer
}

func DamerauLevenshtainMatrixExpose(s1, s2 []rune) (answer int) {
	firstLenght := len(s1)
	secondLenght := len(s2)
	if firstLenght == 0 && secondLenght > 0 {
		answer = secondLenght
	} else if secondLenght == 0 && firstLenght > 0 {
		answer = firstLenght
	} else {
		matrix := make([][]int, len(s1) + 1)
		for i := 0; i <= len(s1); i++ {
			matrix[i] = make([]int, len(s2) + 1)
		}

		for i := 0; i < secondLenght+1; i++ {
			matrix[0][i] = i
		}
		for i := 1; i < firstLenght+1; i++ {
			matrix[i][0] = i
			for j := 1; j < secondLenght+1; j++ {
				if matrix[i-1][j] < matrix[i-1][j-1] {
					matrix[i][j] = matrix[i-1][j]
				} else {
					matrix[i][j] = matrix[i][j-1]
				}
				matrix[i][j]++
				diagonalStep := matrix[i-1][j-1]
				if s1[i-1] != s2[j-1] {
					diagonalStep++
				}
				if diagonalStep < matrix[i][j] {
					matrix[i][j] = diagonalStep
				}
				if (i > 1 && j > 1 && s1[i - 1] == s2[j - 2] && s1[i - 2] == s2[j - 1]) {
					if (matrix[i - 2][j - 2] + 1 < matrix[i][j]) {
						matrix[i][j] = matrix[i - 2][j - 2] + 1
					}
				}
			}
		}
		answer = matrix[firstLenght][secondLenght]
		fmt.Printf("%6s%6s", "", "0")
		for i := 0; i < secondLenght; i++ {
			fmt.Printf("%6s", string(s2[i]))
		}
		fmt.Printf("\n%6s", "0")
		for j := 0; j <= secondLenght; j++ {
			fmt.Printf("%6d", matrix[0][j])
		}
		fmt.Printf("\n")
		for i := 1; i <= firstLenght; i++ {
			fmt.Printf("%6s", string(s1[i - 1]))
			for j := 0; j <= secondLenght; j++ {
				fmt.Printf("%6d", matrix[i][j])
			}
			fmt.Printf("\n")
		}
	}
	return answer
}