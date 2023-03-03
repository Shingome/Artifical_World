from socket_manager import SocketManager
from agent import CategoryAgent
import time


class DataManager:
    def __init__(self):
        self.socket = SocketManager("127.0.0.1", 8000, 8001)
        self.agents = list(CategoryAgent(self) for i in range(6))

    def choose_agent(self, data):
        self.agents[data[0]].store_data(data)

    def start(self):
        while True:
            data = self.socket.get_data()

            if data is not None:
                self.choose_agent(data)

            time.sleep(1)


