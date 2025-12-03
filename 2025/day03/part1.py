
def get_first_largest_battery(bank: str) -> (int, int):
    pos = 0
    largest_battery = int(bank[pos])
    for i in range(len(bank)):
        battery = int(bank[i])
        if battery > largest_battery and i < len(bank) - 1:
            largest_battery = battery
            pos = i
    return largest_battery, pos

def get_second_largest_battery(bank: str) -> (int, int):
    pos = 0
    largest_battery = int(bank[pos])
    for i in range(len(bank)):
        battery = int(bank[i])
        if battery > largest_battery:
            largest_battery = battery
            pos = i
    return largest_battery, pos

banks = open("input.txt").readlines()

total = 0
for bank in banks:
    first_res = get_first_largest_battery(bank.strip())

    second_res = get_second_largest_battery(bank[ first_res[1] + 1: ].strip())

    largest_jolt = str(first_res[0]) + str(second_res[0])
    print(largest_jolt)
    total += int(largest_jolt)

print("The answer is: " + str(total))

