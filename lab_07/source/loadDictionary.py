import requests # Модуль для обработки URL
from lxml import etree
import lxml.html
import csv

url = "https://puzzle-english.com/directory/1000-popular-words"

data = requests.get(url)
tree = lxml.html.document_fromstring(data.text)

f = open("dictionary.txt", "w")
for i in range(999):
    num = i + 1 
    text = tree.xpath(f'/html/body/div[4]/div/div[3]/div[1]/div[3]/div[1]/div/div/ol/li[{num}]/span[1]/text()')
    f.write(text[0].split('[')[0].strip() + "\n")

f.close()