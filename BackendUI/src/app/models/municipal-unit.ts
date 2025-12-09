export interface UnitStaff {
    name: string;
    title: string;
    imageUrl?: string | null;
}

export interface MunicipalUnit {
    id: string;
    title: string;
    content?: string;
    icon: string;
    slug: string;
    staff?: UnitStaff[] | null;
}
