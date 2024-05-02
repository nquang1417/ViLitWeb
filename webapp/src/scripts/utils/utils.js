export function pad(num, size) {
    num = num.toString()
    return num.padStart(size, '0')
}

import dayjs from "dayjs"
import duration from "dayjs/plugin/duration"
export function getRemainingTime(datetime) {
    var diff = dayjs().diff(dayjs(datetime))
    dayjs.extend(duration)
    var timeDiff = dayjs.duration(diff)
    if (timeDiff.years() > 0) {
        return `${timeDiff.years()} năm`
    } else if (timeDiff.months() > 0) {
        return `${timeDiff.months()} tháng`
    } else if (timeDiff.days() > 0) {
        return `${timeDiff.days()} ngày`
    } else if (timeDiff.hours() > 0) {
        return `${timeDiff.hours()} giờ`
    } else if (timeDiff.minutes() > 0) {
        return `${timeDiff.minutes()} phút`
    } else if (timeDiff.seconds() > 0) {
        return `${timeDiff.seconds()} giây`
    } 
}

export function getBookStatusStr(status) {
    switch (status) {
        case 0:
            return "Tạm ngưng"
        case 1:
            return "Đang tiến hành"
        case 2:
            return "Hoàn thành"
        default:
            return "Unknown"
    }
}
