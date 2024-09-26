# -*- coding: utf-8 -*-
list_input = [[0,1,2,"b",4],["1"]]
from pyrevit import script

output = script.get_output()

output.print_code("""
list_input = [[0,1,2,"b",4],["1"]]
print (list_input[0][3])
=> Lấy index 0 của list_input (đặt tên là A), sau đó lấy index 3 của A => In ra giá trị "b"
""")

print ('Đây là cách lấy giá trị của List lồng List, Value lấy được là: ' + str(list_input[0][3]))
# => Lấy index 0 của list_input (đặt tên là A), sau đó lấy index 3 của A => In ra giá trị "b"