
def parse_lines(lines: list[str]) -> list[str]:
    output = []

    for line in lines:
        normalized_line = line.strip()
        parts = normalized_line.split(" ")

        row = []
        for part in parts:
            if part.strip() == "":
                continue

            row.append(part.strip())
        print(row)
        output.append(row)
    return output

def parse_lines_v2(lines: list[str]) -> list[list[str]]:
    output = []

    for line in lines:
        temp = []
        for char in line.replace("\n", ""):
            temp.append(char)
        print(temp)
        output.append(temp)

    # Copy the operators across to make life easier later
    col = 1
    cols = len(output[0])
    currentOperation = output[-1][0]
    while col < cols:
        if is_separator_col(col, output):
            col += 1
            currentOperation = output[-1][col]
            continue
        output[-1][col] = currentOperation
        col += 1
    print(output[-1])

    return output

def is_separator_col(col: int, sheet: list[list[str]]) -> bool:
    rows = len(sheet)
    if col >= len(sheet[0]):
        return True

    for row in range(rows):
        if sheet[row][col] != " ":
            return False
    return True

def get_number_from_column(col: int, sheet: list[list[str]]) -> int:
    rows = len(sheet)

    temp = ""
    for row in range(rows - 1):
        temp += sheet[row][col]
    return int(temp)

raw_lines = open("input.txt").readlines()

lines = parse_lines_v2(raw_lines)
#print(lines)

rows = len(lines)
cols = len(lines[0])

answers = []

col = 0
while col < cols:
    if is_separator_col(col, lines):
        continue

    answer = get_number_from_column(col, lines)
    operation = lines[-1][col]
    col2 = col + 1
    while col2 < cols + 1:
        if is_separator_col(col2, lines):
            answers.append(answer)
            col = col2
            break
        val = get_number_from_column(col2, lines)
        if operation == "+":
            answer += val
        if operation == "*":
            answer *= val

        col2 += 1

    col += 1

print(answers)

print("The answer is {}".format(sum(answers)))