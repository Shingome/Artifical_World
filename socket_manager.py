import socket
from threading import Thread
import numpy as np
from time import sleep


class SocketManager:
    def __init__(self, ip, send_port, get_port):
        self.ip = ip
        self.send_port = send_port
        self.get_port = get_port
        self.data_got = False
        self.data_reseive = None
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.socket.bind((ip, get_port))
        self.process = Thread(target=self.read_thread, daemon=True)
        self.process.start()

    def send_action(self, data: (int, np.array)):
        def transform(data):
            return f"{data[0]} {np.argmax(data[1])}"

        self.socket.sendto(bytes(transform(data), 'utf-8'), (self.ip, self.send_port))

    def get_data(self):
        def transform(data: str):
            return list(float(i) for i in data.split(" "))

        try:
            data, ch = self.socket.recvfrom(1024)
            print(data, ch)
            data = data.decode('utf-8')
            return transform(data)
        except:
            pass

    def read_thread(self):
        self.data_got = False
        while True:
            sleep(5)
            data = self.get_data()
            self.data_reseive = data
            self.data_got = True

    def read_get_data(self):
        data = None
        if self.data_got:
            self.data_got = False
            data = self.data_reseive
            self.data_reseive = None
        return data
