import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable(
  // {  providedIn: 'root'}
  )
export class EventoService {
  baseUrl = 'https://localhost:5001/api/eventos';
  constructor(private http: HttpClient) { }

  public getEvento(): Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseUrl)
  }

  public getEventoByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseUrl}/tema/${tema}`)
  }

  public getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseUrl}/${id}`)
  }

  public post(evento: Evento): Observable<Evento>{
    return this.http.post<Evento>(this.baseUrl, evento)
  }

  public put(evento: Evento): Observable<Evento>{
    return this.http.put<Evento>(`${this.baseUrl}/${evento.id}`, evento)
  }

  public deleteEvento(id: number): Observable<any>{
    return this.http.delete<string>(`${this.baseUrl}/${id}`)
  }
}
