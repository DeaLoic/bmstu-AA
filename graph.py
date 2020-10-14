import matplotlib.pyplot as plt


x1 = [2, 52, 102, 152, 202, 252]
x2 = [302, 352, 402, 452, 502]

y11 = [16, 2860, 18265, 59217, 134267, 277382]
y12 = [6, 2060, 15489, 49514, 113178, 242845]
y13 = [6, 1881, 12675, 39314, 90281, 179143]

y21 = [450825, 728434, 1090420, 1563860, 2121280]
y22 = [384745, 613048, 927255, 1303610, 1772410]
y23 = [309060, 520842, 736226, 1044170, 1444390]

fig = plt.figure()

plt.plot(x1, y11, color="red", label="Классический")
plt.plot(x1, y12, color="blue", label="Винограда")
plt.plot(x1, y13, color="yellow", label="Винограда оптимизированный")

'''
plt.plot(x2, y21, color="red", label="Классический")
plt.plot(x2, y22, color="blue", label="Винограда")
plt.plot(x2, y23, color="yellow", label="Винограда оптимизированный")
'''
plt.legend()

plt.title("Результаты эксперимента")
plt.xlabel("Сторона квадратной матрицы")
plt.ylabel("Микросекунды")
plt.grid(True)

plt.show()
legend.show()