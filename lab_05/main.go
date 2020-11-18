package main

import (
	"fmt"
	"sync"
	"time"

	queue "aa/lab_05/queue"
)

const MAX_ELEMENT = 100

var wg sync.WaitGroup
var log []string

type SetTimestamp func(*queue.Element, int64)

func pingPlace(id int, message string, time int64) {
	log = append(log, fmt.Sprintf("%d     %s    %d\n", id, message, time))
}

func doStuff() {
	time.Sleep(time.Millisecond * 1000)
}

func startHandle(queueFirst *queue.Queue) {
	defer wg.Done()
	for i := 0; i < MAX_ELEMENT; {
		element := new(queue.Element)
		element.ID = i
		queueFirst.Mu.Lock()
		queueFirst.Push(element)
		queueFirst.Mu.Unlock()
		timeNow := time.Now().UnixNano()
		queue.SetFI(element, timeNow)
		pingPlace(element.ID, fmt.Sprintf("  Insert %d  ", 1), timeNow)
		i++
	}
}

func handleQueue(firstQueue *queue.Queue, secondQueue *queue.Queue, stLeave SetTimestamp, stIn SetTimestamp, queueNumber int) {
	defer wg.Done()
	for i := 0; i < MAX_ELEMENT; {
		firstQueue.Mu.Lock()
		element := firstQueue.Pop()
		firstQueue.Mu.Unlock()
		if element != nil {
			timeNow := time.Now().UnixNano()
			stLeave(element, timeNow)
			pingPlace(element.ID, fmt.Sprintf("  Leave %d   ", queueNumber), timeNow)
			doStuff()
			secondQueue.Mu.Lock()
			secondQueue.Push(element)
			secondQueue.Mu.Unlock()
			timeNow = time.Now().UnixNano()
			stIn(element, timeNow)
			pingPlace(element.ID, fmt.Sprintf("  Insert %d  ", queueNumber+1), timeNow)
			i++
		}
	}
}

func main() {
	firstQueue := queue.Queue{}
	secondQueue := queue.Queue{}
	thirdQueue := queue.Queue{}
	answerQueue := queue.Queue{}

	wg.Add(1)
	go handleQueue(&firstQueue, &secondQueue, queue.SetFO, queue.SetSI, 1)
	wg.Add(1)
	go handleQueue(&secondQueue, &thirdQueue, queue.SetSO, queue.SetTI, 2)
	wg.Add(1)
	go handleQueue(&thirdQueue, &answerQueue, queue.SetTO, queue.SetA, 3)
	wg.Add(1)
	go startHandle(&firstQueue)

	wg.Wait()

	for i := 0; i < len(log); i++ {
		fmt.Println(log[i])
	}
}
