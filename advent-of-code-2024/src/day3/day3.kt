package day3

import java.io.File

fun main() {
    var sum = 0;

    val configs = listOf<Pair<List<(
        text: String,
        i: Int
    ) -> Boolean>, Int>>(
        Pair(listOf(
            { text, i -> text[i] == 'm' },
            { text, i -> text[i] == 'u' },
            { text, i -> text[i] == 'l' },
            { text, i -> text[i] == '(' },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i] == ',' },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i].isDigit() },
            { text, i -> text[i] == ')' },
            { text, i ->
                val regex = "mul\\((\\d{1,3}),(\\d{1,3})\\)";
                text
                    .substring((i - 12).coerceAtLeast(0), i)
                    .let { regex.toRegex().findAll(it).toList() }
                    .map { it.groupValues[1].toInt() to it.groupValues[2].toInt() }
                    .sumOf { it.first * it.second }
                    .run {
                        sum += this
                    }

                return@listOf true;
            }
        ), 3)
    )
    val configIndexes = configs.map { 0 }.toMutableList();

    println(configIndexes)

    val text = File("src/day3/input")
        .readLines()
        .joinToString()

    text.indices.forEach { i ->
        configIndexes
            .mapIndexed { indexx, it -> configs[indexx] to it }
            .forEachIndexed { indexx, x ->
            val config = x.first;
            val configIndex = x.second;

            (configIndex..(configIndex + config.second).coerceAtMost(config.first.count() - 1)).forEach { cIndex ->
                if (config.first[cIndex](text, i)) {
                    configIndexes[indexx] = cIndex + 1
                    if (configIndexes[indexx] == config.first.count())
                        configIndexes[indexx] = 0
                }
            }
        }
    }

    println(sum)
}
