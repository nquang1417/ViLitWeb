export function pad(num, size) {
    num = num.toString()
    return num.padStart(size, '0')
}

export function getRemainingTime(datetime) {
    let remainingTime = {
        days: "0",
        hours: "0",
        minutes: "0",
        seconds: "0",
        percentage: 0.1,
        timeleft: false
    }

    const today = new Date()
    const lastUpdate = new Date(datetime)
    var timeDiff = today.getTime() - lastUpdate.getTime()
    if (timeDiff > 0) {
        remainingTime = {
          days: Math.floor(timeDiff / (1000 * 60 * 60 * 24)),
          hours: Math.floor((timeDiff / (1000 * 60 * 60)) % 24),
          minutes: Math.floor((timeDiff / 1000 / 60) % 60),
          seconds: Math.floor((timeDiff / 1000) % 60),
        };
      }
    // console.log(today,' - ', lastUpdate,)
    // console.log(remainingTime)
    if (remainingTime.days > 0) {
        return `${remainingTime.days.toString()} ngày`
    } else if (remainingTime.hours > 0) {
        return `${remainingTime.hours.toString()} giờ`
    } else if (remainingTime.minutes > 0) {
        return `${remainingTime.minutes.toString()} phút`
    }
    return `${remainingTime.seconds.toString()} giây`;
}