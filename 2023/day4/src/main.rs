use std::collections::LinkedList;
use std::fs;

fn main() {
    println!("Hello, world!");

    part_one("sample.txt".to_string());
}

fn part_one(filename: String) {
    let lines = fs::read_to_string(filename)
        .expect("Unable to open and read the file");

    for line in lines.split("\n") {
        println!("{line}");
    }
}

fn get_winning_numbers(line: String) -> LinkedList<i32> {
    let colon_index = line.chars().position(|c| c == ':').unwrap();
    let separator_index = line.chars().position(|c| c == '|').unwrap();
}