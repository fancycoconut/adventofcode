
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
        #print(row)
        output.append(row)
    return output

raw_lines = open("input.txt").readlines()

lines = parse_lines(raw_lines)

rows = len(lines)
cols = len(lines[0])

answers = []
for col in range(cols):
    operation = lines[-1][col]

    answer = int(lines[0][col])
    for row in range(1, rows - 1):
        val = int(lines[row][col])
        #print(val)

        if operation == "+":
            answer += val
        else:
            answer *= val
    answers.append(answer)
print(answers)

print("The answer is {}".format(sum(answers)))