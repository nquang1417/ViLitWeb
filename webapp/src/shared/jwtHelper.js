// const jwt = require('jsonwebtoken')

export function jwtDecrypt(token) {
    var base64Url = token.split(".")[1];
    var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    var jsonPayload = decodeURIComponent(
        atob(base64)
            .split("")
            .map(function (c) {
                return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
            })
            .join("")
    );

    return JSON.parse(jsonPayload);
}

// export function jwtDecode(token) {
//     return jwt.decode(token, {complete: true})
// }

// export function updateToken(token, addittionalClaims) {
//     const key = 'This is my ViLit Secret key for authentication'
//     const decodedToken = jwt.decode(token, {complete: true})
//     if (!decodedToken || !decodedToken.payload) {
//         console.error('Invalid token');
//         return;
//     }

//     const updatePayload = {
//         ...decodedToken.payload,
//         ...addittionalClaims
//     }
//     const newToken = jwt.sign(updatePayload, key)
//     return newToken

// }
 
export function tokenAlive(exp, role) {
    if (Date.now() >= exp * 1000 && role == "Admin") {
      return false;
    }
    return true;
  }