import bcrypt from 'bcryptjs'

const saltRounds = 10;

export function hashPassword(password) {
    return bcrypt.hashSync(password, saltRounds);
}

export function comparePassword(password, hashedPassword) {
    return bcrypt.compareSync(password, hashedPassword)
}