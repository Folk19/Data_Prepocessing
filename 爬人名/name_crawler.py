 # -*- coding: UTF-8 -*-
__author__ = 'shawn'
import time
from lxml import etree
from selenium import webdriver


def get_content(page, fw):
    page_html = etree.HTML(page)
    row_root = page_html.xpath(".//table[@id='result']/tbody/tr")
    for row in row_root:
        if etree.iselement(row.find("./td[3]")):
            name = row.xpath("./td[3]")[0].text
            if name is None:
                continue
            print name.encode('utf8')
            name += "\n"
            fw.write(name.encode('utf-8'))
        else:
            continue
    return None

fwrite = open("test12345.txt", "w")
chromedriver = "C:\Users\don't\Desktop\chromedriver.exe"
driver = webdriver.Chrome(chromedriver)
driver.get("http://freshman.tw/")

exelem = driver.find_element_by_id("select_exam")
exam_value = exelem.find_elements_by_tag_name("option")
flag = 0
for exam in exam_value:
    if exam.get_attribute("value") != "103-2" and flag == 0:
        continue
    flag = 1
    exam.click()
    print exam.get_attribute("value").encode('utf8')
    time.sleep(3)
    selem = driver.find_elements_by_id("select_school")
    if selem:
        school_value = selem[0].find_elements_by_tag_name("option")
        for school in school_value:
            print school.get_attribute("value").encode('utf8') + " " + school.text.encode('utf8')
            school.click()
            time.sleep(3)
            delem = driver.find_elements_by_id("select_dept")
            if delem:
                dept_value = delem[0].find_elements_by_tag_name("option")
                for dept in dept_value:
                    print dept.get_attribute("value").encode('utf8')
                    dept.click()
                    search = driver.find_element_by_id("buttonSchool")
                    search.click()
                    time.sleep(3)
                    get_content(driver.page_source, fwrite)
driver.close()
fwrite.close()