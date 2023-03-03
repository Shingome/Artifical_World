from model import Agent


class CategoryAgent:
    def __init__(self, manager, state_size=6, action_size=3, random_seed=452):
        self.agent = Agent(state_size, action_size, random_seed)
        self.manager = manager
        self.memory = dict()

    def add_first_part(self, data):
        state = tuple(data[2:])
        action = self.agent.act(data[2:])
        self.memory[data[1]] = [state]
        self.memory[data[1]].append(action)
        self.return_action(data[1], action)

    def add_second_part(self, data):
        self.memory[data[1]].append(data[2])
        self.memory[data[1]].append(tuple(data[3:6]))
        self.memory[data[1]].append(data[6])

    def return_action(self, id, action):
        self.manager.socket.send_action((id, action))

    def store_data(self, data):
        if len(data) == 5:
            self.add_first_part(data)
        elif len(data) == 7:
            self.add_second_part(data)
            if data[6] == 1:
                self.memory.pop(data[1])
            else:
                self.add_first_part([*data[:2], *data[3:6]])
