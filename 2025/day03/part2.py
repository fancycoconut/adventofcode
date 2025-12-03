
def get_best_battery(bank: str, size: int) -> int:
    """ Gets the best battery while preserving the order """
    nums = list(map(int, bank.strip()))

    start = 0
    max_jolts = []
    for i in range(size):
        # We need size-k more digits, so can only look at positions
        # that leave enough remaining digits
        end = len(nums) - (size - i) + 1
        chunk = nums[start:end]

        # Greedily pick the largest digit in valid range
        max_digit = max(chunk)
        max_jolts.append(max_digit)

        # Next search starts after the chosen digit
        start = start + chunk.index(max_digit) + 1
    return int(''.join(map(str, max_jolts)))


banks = open("input.txt").readlines()

total = 0
for bank in banks:
    largest_jolt = get_best_battery(bank, 12)

    print(largest_jolt)
    total += int(largest_jolt)

print("The answer is: " + str(total))

