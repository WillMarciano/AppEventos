import { Evento } from "./Evento";
import { RedeSocial } from "./RedeSocial";
import { User } from "./identity/User";

export interface Palestrante {
    id: number;
    miniCurriculo: string;
    user: User;
    redesSociais: RedeSocial[];
    palestrantesEventos: Evento[];    
}
