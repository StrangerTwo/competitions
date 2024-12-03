package day1

import java.io.File
import kotlin.math.absoluteValue

fun main() {
    val xs = ArrayList<Int>();
    val ys = ArrayList<Int>();

    File("src/day1/input")
        .readLines()
        .map { line -> line.split("\\s+".toRegex()) }
        .forEach {
            xs.add(it[0].toInt())
            ys.add(it[1].toInt())
        }

    xs.sortDescending()
    ys.sortDescending()

    val result1 = (0..<xs.size)
        .map { xs[it] - ys[it] }
        .sumOf { it.absoluteValue }

    println(result1)

    val result2 = (0..<xs.size)
        .map { i -> xs[i] * ys.count { it == xs[i] } }
        .sum()

    println(result2)
}