from unicodedata import normalize


def read_map(filename: str) -> [ str, str]:
    map = []
    lines = open(filename).readlines()

    for line in lines:
        normalized_line = line.strip()

        row = []
        for char in normalized_line:
            row.append(char)
        map.append(row)
    return map

def print_map(map: [str, str]) -> None:
    rows = len(map)
    cols = len(map[0])

    for row in range(rows):
        line = ""
        for col in range(cols):
            line += map[row][col]
        print(line)
    print()

def get_num_of_adjacent_toilet_rolls(pos: (int, int), map: [str, str]) -> int:
    num_of_toilet_rolls = 0
    rows = len(map)
    cols = len(map[0])

    row = pos[0]
    col = pos[1]

    if map[row][col] != '@':
        return 99

    # top left
    if row - 1 >= 0 and col - 1 >= 0:
        num_of_toilet_rolls += 1 if map[row-1][col-1] == '@' else 0
    # top mid
    if row -1 >= 0:
        num_of_toilet_rolls += 1 if map[row - 1][col] == '@' else 0
    # top right
    if row - 1 >= 0 and col + 1 < cols:
        num_of_toilet_rolls += 1 if map[row-1][col+1] == '@' else 0
    # left
    if col - 1 >= 0:
        num_of_toilet_rolls += 1 if map[row][col-1] == '@' else 0
    # right
    if col + 1 < cols:
        num_of_toilet_rolls += 1 if map[row][col+1] == '@' else 0
    # bottom left
    if row + 1 < rows and col - 1 >= 0:
        num_of_toilet_rolls += 1 if map[row+1][col-1] == '@' else 0
    # bottom mid
    if row + 1 < rows:
        num_of_toilet_rolls += 1 if map[row+1][col] == '@' else 0
    # bottom right
    if row + 1 < rows and col + 1 < cols:
        num_of_toilet_rolls += 1 if map[row + 1][col + 1] == '@' else 0

    return num_of_toilet_rolls

def get_accessible_toilet_roll_positions(map: [str, str]) -> [(int, int)]:
    output = []
    rows = len(map)
    cols = len(map[0])

    for row in range(rows):
        for col in range(cols):
            adjacent_toilet_rolls = get_num_of_adjacent_toilet_rolls((row, col), map)
            #print("(" + str(row) + ", " + str(col) + ") - " + str(adjacent_toilet_rolls))

            if adjacent_toilet_rolls < 4:
                output.append((row, col))
    return output

toilet_rolls_map = read_map("sample.txt")

accessible_toilet_rolls = get_accessible_toilet_roll_positions(toilet_rolls_map)

total_removed = 0
while len(accessible_toilet_rolls) > 0:
    for pos in accessible_toilet_rolls:
        row = pos[0]
        col = pos[1]
        toilet_rolls_map[row][col] = '.'
        total_removed += 1

    print_map(toilet_rolls_map)
    accessible_toilet_rolls = get_accessible_toilet_roll_positions(toilet_rolls_map)

print(total_removed)