# -*- coding: utf-8 -*-
'''
# Định nghĩa lớp cha
class Class1:
    def __init__(self, value):
        self.value = value

    def display(self):
        print("Class1 value: {}".format(self.value))

# Định nghĩa lớp con kế thừa từ Class1
class Class2(Class1):
    def __init__(self, value, extra_value):
        Class1.__init__(self, value)  # Gọi hàm khởi tạo của lớp cha trực tiếp
        self.extra_value = extra_value

    def display(self):
        Class1.display(self)  # Gọi phương thức display của lớp cha trực tiếp
        print("Class2 extra_value: {}".format(self.extra_value))

# Sử dụng lớp con
instance = Class2(10, 20)
instance.display()

'''


'''
list_input = [[0,1,2,"b",4],["1"]]
print list_input[0][3]
# => Lấy index 0 của list_input (đặt tên là A), sau đó lấy index 3 của A => In ra giá trị "b"
'''