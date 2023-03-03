import copy
import random
import numpy as np


class OUNoise:
    def __init__(self, size, seed, mu=0.0, theta=0.1, sigma=.5, sigma_min = 0.05, sigma_decay=.99):
        random.seed(seed)
        self.mu = mu * np.ones(size)
        self.theta = theta
        self.sigma = sigma
        self.sigma_min = sigma_min
        self.sigma_decay = sigma_decay
        self.size = size
        self.state = None
        self.reset()

    def reset(self):
        self.state = copy.copy(self.mu)
        self.sigma = max(self.sigma_min, self.sigma*self.sigma_decay)

    def sample(self):
        x = self.state
        dx = self.theta * (self.mu - x) + self.sigma * np.random.standard_normal(self.size)
        self.state = x + dx
        return self.state