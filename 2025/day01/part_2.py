
class Node:
    def __init__(self, val, next = None, prev = None):
        self.val = val
        self.next = next
        self.prev = prev

## Part 1
input_file = "input.txt"
lines = open(input_file).readlines()
print(lines)

maxRotations = 99

# Generate a circular linked list
root = Node(0)
current = root

starting_pos = 50
starting_node = root

for i in range(1, maxRotations + 1):
    next_node = Node(i)
    if i == starting_pos:
        starting_node = next_node
    next_node.prev = current
    current.next = next_node
    current = next_node
current.next = root
root.prev = current

print('The dial is at: ' + str(starting_node.val))

current = starting_node
password_ticks = 0
for line in lines:
    normalized_line = line.replace('\n', '')
    #print('Before: ' + str(current.val) + ' - ' + normalized_line)
    direction = normalized_line[0]
    num_of_rotations = int(normalized_line[1:])

    if direction == 'R':
        for i in range(num_of_rotations):
            current = current.next
            if current.val == 0:
                password_ticks += 1
    else:
        for i in range(num_of_rotations):
            current = current.prev
            if current.val == 0:
                password_ticks += 1
    #print('The dial is at: ' + str(current.val))

print(password_ticks)

