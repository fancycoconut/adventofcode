
def get_id_ranges(lines: list[str]) -> list[str]:
    output = []
    for line in lines:
        normalized_line = line.replace("\n", "")
        segments = normalized_line.split(",")
        for segment in segments:
            if segment == "":
                continue
            output.append(segment)
    return output

def split_to_chunks(input: str, chunk_size: int) -> list[str]:
    output = []

    i = 0
    temp = ""
    while i < len(input):
        char = input[i]
        if len(temp) == chunk_size:
            output += [temp]
            temp = ""
            continue

        temp += char
        i += 1
    output += [temp]
    #print(output)
    return output

def is_invalid_id(id: str) -> bool:
    mid = len(id) // 2
    return id[0:mid] == id[mid:]

def is_invalid_id_v2(id: str) -> bool:
    mid = len(id) // 2
    for i in range(1, mid + 1):
        chunks = split_to_chunks(id, i)
        if len(set(chunks)) == 1:
            return True
    return False

#print(is_invalid_id_v2("565656"))

lines = open("input.txt").readlines()

id_ranges = get_id_ranges(lines)

invalid_ids = []
for id_range in id_ranges:
    parts = id_range.split("-")

    start = int(parts[0])
    end = int(parts[1])

    # Check if id is invalid
    for i in range(start, end + 1):
        if is_invalid_id(str(i)) or is_invalid_id_v2(str(i)):
            invalid_ids.append(i)

        #print(str(i) + " is valid: " + str(is_invalid))

print(invalid_ids)

sum_of_invalid_ids = sum(invalid_ids)
print(sum_of_invalid_ids)