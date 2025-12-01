export interface UnitStaff {
    name: string;
    title: string;
    imageUrl?: string;
}

export interface MunicipalUnit {
    id: string;
    title: string;
    content?: string;
    icon?: string;
    staff?: UnitStaff[];
}
