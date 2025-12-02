
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

def is_invalid_id(id: str) -> bool:
    mid = len(id) // 2
    return id[0:mid] == id[mid:]

lines = open("input.txt").readlines()

id_ranges = get_id_ranges(lines)

invalid_ids = []
for id_range in id_ranges:
    parts = id_range.split("-")

    start = int(parts[0])
    end = int(parts[1])

    # Check if id is invalid
    for i in range(start, end + 1):
        is_invalid = is_invalid_id(str(i))
        if is_invalid:
            invalid_ids.append(i)

        print(str(i) + " is valid: " + str(is_invalid))

print(invalid_ids)

sum_of_invalid_ids = sum(invalid_ids)
print(sum_of_invalid_ids)