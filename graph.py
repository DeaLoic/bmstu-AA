import matplotlib.pyplot as plt


x1 = [2, 52, 102, 152, 202, 252]
x2 = [302, 352, 402, 452, 502]

y11 = [1, 22, 69, 161, 287, 410]
y12 = [1, 15, 44, 103, 159, 229]
y13 = [1, 9, 26, 51, 104, 131]

y21 = [623, 886, 1254, 1557, 1715]
y22 = [326, 453, 554, 704, 887]
y23 = [212, 279, 366, 466, 583]

fig = plt.figure()

label1 = "Пузырек"
label2 = "Выбор"
label3 = "Вставки"

plt.plot(x1, y11, color="red", label=label1)
plt.plot(x1, y12, color="blue", label=label2)
plt.plot(x1, y13, color="yellow", label=label3)

'''
plt.plot(x2, y21, color="red", label=label1)
plt.plot(x2, y22, color="blue", label=label2)
plt.plot(x2, y23, color="yellow", label=label3)
'''

plt.legend()

plt.title("Результаты эксперимента")
plt.xlabel("Размер массива")
plt.ylabel("Микросекунды")
plt.grid(True)

plt.show()
legend.show()