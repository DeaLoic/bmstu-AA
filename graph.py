import matplotlib.pyplot as plt

test1 = "./binary.txt"
test2 = "./brute_force.txt"
test3 = "./segments.txt"


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

fig = plt.figure()

label1 = "Бинарный поиск"
label2 = "Полный перебор"
label3 = "Поиск по сегментам"

plt.plot(x1, y1, color="red", label=label1)
plt.plot(x1, y2, color="blue", label=label2)
plt.plot(x1, y3, color="yellow", label=label3)

plt.legend()

plt.title("Результаты эксперимента")
plt.xlabel("Порядковый номер элемента")
plt.ylabel("Тики")
plt.grid(True)

plt.show()
legend.show()