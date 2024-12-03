package day2

import java.io.File
import kotlin.math.absoluteValue

fun main() {
    File("src/day2/input")
        .readLines()
        .map { line -> line.split("\\s+".toRegex()) }
        .map { it.map(String::toInt) }
        .map {
            calc(it) == -1 || {
                it.indices.any { i ->
                    calc(it.toMutableList().apply { removeAt(i) }) == -1
                }
            }()
        }
        .count { it }
        .let { println(it) }

}

private fun calc(it: List<Int>): Int {
    var allAsc: Boolean? = null;
    for (i in 1..<it.size) {
        if ((it[i] - it[i - 1]).absoluteValue !in 1..3)
            return i

        val isAsc = (it[i] - it[i - 1]).let { x -> x.absoluteValue == x };
        if (allAsc == null) allAsc = isAsc;
        if (isAsc != allAsc)
            return i
    }

    return -1
}