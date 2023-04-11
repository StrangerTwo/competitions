const fs = require('fs');
const inquirer = require('inquirer')

const log = fs.readFileSync('tvlog.csv').toString();

const lines = log.split('\n');

const logs = lines.map(x => {
    const y = x.split(';');
    return {
        id: y[0],
        channel: y[1],
        time: new Date(y[2]),
        duration: parseInt(y[3])
    }
})

const months = {
    'květen 2018': new Date(2018, 4),
    'červen 2018': new Date(2018, 5),
    'červenec 2018': new Date(2018, 6),
    'srpen 2018': new Date(2018, 7),
    'září 2018': new Date(2018, 8),
    'říjen 2018': new Date(2018, 9),
    'listopad 2018': new Date(2018, 10),
    'prosinec 2018': new Date(2018, 11),
    'leden 2019': new Date(2019, 0),
    'únor 2019': new Date(2019, 1),
}

inquirer
  .prompt([
    {
      type: 'list',
      name: 'month',
      message: 'Vyber měsíc',
      choices: Object.keys(months)
    },
  ])
  .then((answers) => {
    const {month} = answers;
    
    const from = months[month];
    const to = new Date(from)
    to.setMonth(from.getMonth() + 1)

    watchTime(from, to); 
  });



function watchTime(from, to) {
  const monthLogs = logs.filter(x => x.time.getTime() >= from.getTime() && x.time.getTime() <= to.getTime());

  console.log(monthLogs.length, logs.length)

  const watchTimes = {}

  for (const log of monthLogs) {
    watchTimes[log.channel] = watchTimes[log.channel] ?? 0
    watchTimes[log.channel] += log.duration
  }

  console.log(watchTimes)

  const sorted = Object.entries(watchTimes).sort(([channel, duration], [channelB, durationB]) => durationB - duration);

  for (const [channel, duration] of sorted) {
    console.log(`${formatSeconds(duration)} - ${channel}`);
  }
}

function formatSeconds(seconds) {
  const days = Math.floor(seconds / (24 * 60 * 60)) 
  const hours = Math.floor((seconds % (24 * 60 * 60)) / (60 * 60))
  const minutes = Math.floor((seconds % (60 * 60)) / (60)) 
  seconds = seconds % 60;

  return `${days}d ${hours}:${minutes}:${seconds}`
}