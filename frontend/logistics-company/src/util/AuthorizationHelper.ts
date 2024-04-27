export function isAuthorized(jwt: string){  
    return jwt != null;
}

export function isAuthorizedForRole(currentRole: string, expectedRole: string){
    return currentRole === expectedRole;
}