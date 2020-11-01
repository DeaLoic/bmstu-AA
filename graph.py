import matplotlib.pyplot as plt

test1 = "./testClassicFix.txt"
test2 = "./testParallelFirstFix.txt"
test3 = "./testParallelSecondFix.txt"

test4 = "./testClassicMulty.txt"
test5 = "./testParallelFirstMulty.txt"
test6 = "./testParallelSecondMulty.txt"

x1 = []
y1 = []
y2 = []
y3 = []

with open(test1, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        x1.append(line[0])
        y1.append(line[1])

with open(test2, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        y2.append(line[1])

with open(test3, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        y3.append(line[1])

x2 = []
y4 = []
y5 = []
y6 = []

with open(test4, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        x2.append(line[0])
        y4.append(line[1])

with open(test5, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        y5.append(line[1])

with open(test6, mode="r") as f:
    for line in f:
        line = list(map(int, line.strip().split()))
        y6.append(line[1])

fig = plt.figure()

label1 = "Последовательный"
label2 = "Параллельный 1"
label3 = "Параллельный 2"

'''
plt.plot(x1, y1, color="red", label=label1)
plt.plot(x1, y2, color="blue", label=label2)
plt.plot(x1, y3, color="yellow", label=label3)

'''
plt.plot(x2, y4, color="red", label=label1)
plt.plot(x2, y5, color="blue", label=label2)
plt.plot(x2, y6, color="yellow", label=label3)

plt.legend()

plt.title("Результаты эксперимента")
plt.xlabel("Число потоков")
plt.ylabel("Миллисекунды")
plt.grid(True)

plt.show()
legend.show()