
def get_fresh_ingredients_id_ranges(lines: list[str]) -> list[(int, int)]:
    output = []
    for line in lines:
        parts = line.strip().split('-')
        output.append((int(parts[0]), int(parts[1])))

    return output

lines = open("input.txt").readlines()
sectionBreakIndex = lines.index("\n")
ingredient_range_lines = lines[:sectionBreakIndex]

fresh_ingredient_ranges = get_fresh_ingredients_id_ranges(ingredient_range_lines)

# merge the ranges first
fresh_ingredient_ranges.sort(key=lambda tup: tup[0])
print(fresh_ingredient_ranges)

merged_ingredients = []

i = 1
working_interval = fresh_ingredient_ranges[0]
while i < len(fresh_ingredient_ranges):
    interval = fresh_ingredient_ranges[i]

    if interval[0] > working_interval[1]:
        merged_ingredients.append(working_interval)
        working_interval = interval
    else:
        start = working_interval[0]
        end = max(working_interval[1], interval[1])
        working_interval = (start, end)
    i += 1
merged_ingredients.append(working_interval)

print(merged_ingredients)

# do math
totalFreshIngredients = 0
for ingredient in merged_ingredients:
    totalFreshIngredients += ingredient[1] - ingredient[0] + 1

print("The answer is {}".format(totalFreshIngredients))
