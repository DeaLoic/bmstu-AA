import matplotlib.pyplot as plt


x1 = [2, 52, 102, 152, 202, 252]
x2 = [302, 352, 402, 452, 502]

y11 = [1, 22, 87, 222, 344, 522]
y12 = [1, 14, 58, 122, 170, 283]
y13 = [1, 9, 31, 66, 117, 193]

y21 = [761, 1014, 1419, 1930, 2124]
y22 = [380, 492, 656, 815, 1016]
y23 = [250, 367, 492, 577, 663]

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