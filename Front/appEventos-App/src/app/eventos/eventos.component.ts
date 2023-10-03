import { Component, OnInit } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  // providers: [EventoService]
})
export class EventosComponent implements OnInit {
  public events: Evento[] = [];
  public filteredEvents: Evento[] = [];

  public widthImg: number = 150;
  public marginImg: number = 2;
  public showImg: boolean = true;
  private _filterList: string = '';

  public get filterList(): string {
    return this._filterList;
  }

  public set filterList(value: string) {
    this._filterList = value;
    this.filteredEvents = this.filterList
      ? this.filterEvents(this.filterList)
      : this.events;
  }

  constructor(private eventoService: EventoService) {}

  public ngOnInit(): void {
    this.getEventos();
  }

  public alterImage(): void {
    this.showImg = !this.showImg;
  }

  public getEventos(): void {
    this.eventoService.getEvento().subscribe(
      (_eventos: Evento[]) => {
        this.events = _eventos;
        this.filteredEvents = this.events;
      },
      (error) => console.log(error)
    );
  }

  public filterEvents(filterFor: string): Evento[] {
    filterFor = filterFor.toLocaleLowerCase();
    return this.events.filter(
      (evento: { tema: string; local: string }) =>
        evento.tema.toLocaleLowerCase().indexOf(filterFor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filterFor) !== -1
    );
  }
}
