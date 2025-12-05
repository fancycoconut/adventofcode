
def get_fresh_ingredients_id_ranges(lines: list[str]) -> list[(int, int)]:
    output = []
    for line in lines:
        parts = line.strip().split('-')
        output.append((int(parts[0]), int(parts[1])))

    return output

lines = open("input.txt").readlines()
sectionBreakIndex = lines.index("\n")
ingredient_range_lines = lines[:sectionBreakIndex]
available_ingredients = lines[sectionBreakIndex+1:]
print(available_ingredients)

fresh_ingredient_ranges = get_fresh_ingredients_id_ranges(ingredient_range_lines)

freshIngredients = set()
for ingredient in available_ingredients:
    normalized_ingredient = int(ingredient.strip())

    for ingredient_range in fresh_ingredient_ranges:
        if normalized_ingredient >= ingredient_range[0] and normalized_ingredient <= ingredient_range[1]:
            freshIngredients.add(normalized_ingredient)
            #print(normalized_ingredient)

print("The answer is {}".format(len(freshIngredients)))
