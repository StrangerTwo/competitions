const delay = (delay: number) => {
    return new Promise((resolve, reject) => {
        setTimeout(resolve, delay)
    })
}

export default delay;