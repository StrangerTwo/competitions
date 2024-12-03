package day3

import java.io.File

fun main() {
    File("src/day3/input")
        .readLines()
        .joinToString()
        .let { "mul\\((\\d{1,3}),(\\d{1,3})\\)".toRegex().findAll(it).toList() }
        .map { it.groupValues[1].toInt() * it.groupValues[2].toInt() }
        .sum()
        .let { println(it) }
}
