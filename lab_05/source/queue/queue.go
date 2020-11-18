package queue

import (
	"sync"
)

type Element struct {
	ID                 int
	TimestampFirstIn   int64
	TimestampFirstOut  int64
	TimestampSecondIn  int64
	TimestampSecondOut int64
	TimestampThirdIn   int64
	TimestampThirdOut  int64
	TimestampAnswer    int64
}

func SetFI(this *Element, timestamp int64) {
	this.TimestampFirstIn = timestamp
}

func SetFO(this *Element, timestamp int64) {
	this.TimestampFirstOut = timestamp
}
func SetSI(this *Element, timestamp int64) {
	this.TimestampSecondIn = timestamp
}
func SetSO(this *Element, timestamp int64) {
	this.TimestampSecondOut = timestamp
}
func SetTI(this *Element, timestamp int64) {
	this.TimestampThirdIn = timestamp
}
func SetTO(this *Element, timestamp int64) {
	this.TimestampThirdOut = timestamp
}
func SetA(this *Element, timestamp int64) {
	this.TimestampAnswer = timestamp
}

type Queue struct {
	Mu    sync.Mutex
	Array []*Element
}

func (this *Queue) Push(element *Element) {
	this.Array = append(this.Array, element)
}

func (this *Queue) Pop() (element *Element) {
	if len(this.Array) > 0 {
		element = this.Array[0]
		this.Array = this.Array[1:]
	}
	return
}
