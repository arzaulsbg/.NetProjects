(() => {
  const grid = document.getElementById('movieGrid');
  const search = document.getElementById('movieSearch');
  const status = document.getElementById('apiStatus');
  const empty = document.getElementById('noMovies');
  const loading = document.getElementById('movieLoading');
  const more = document.getElementById('loadMore');
  let page = 1, mode = 'popular', timer;

  const esc = (v='') => String(v).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#039;'}[c]));
  function card(m){
    const rating = Number(m.voteAverage ?? 0).toFixed(1);
    const poster = m.posterUrl || (m.poster_path ? `https://image.tmdb.org/t/p/w500${m.poster_path}` : '/images/no-poster.svg');
    const year = m.year || (m.release_date ? m.release_date.slice(0,4) : '—');
    const genres = m.genresText || 'Movie';
    return `<div class="movie-card api-movie"><a href="/Home/Tmdb/${m.id}"><div class="poster-wrap"><img src="${esc(poster)}" alt="${esc(m.title)}" loading="lazy"><span class="rating">★ ${rating}</span></div></a><div class="movie-info"><h3>${esc(m.title)}</h3><p>${esc(genres)} <i>•</i> ${esc(year)}</p><div class="card-actions"><a class="ghost small" href="/Home/Tmdb/${m.id}">Details</a><a class="primary small" href="/Booking/Tmdb/${m.id}">Book</a></div></div></div>`;
  }
  async function load(reset=true){
    if(!grid)return;
    if(reset){ page=1; grid.innerHTML=''; }
    loading?.classList.remove('hidden'); empty?.classList.add('hidden'); more?.classList.add('hidden');
    try{
      const q=search?.value.trim() || '';
      mode=q?'search':'popular';
      const endpoint=q?`/api/movies/search?query=${encodeURIComponent(q)}&page=${page}`:`/api/movies/popular?page=${page}`;
      const res=await fetch(endpoint);
      if(!res.ok) throw new Error('API error');
      const movies=await res.json();
      loading?.classList.add('hidden');
      if(reset && !movies.length){ empty?.classList.remove('hidden'); status.textContent='No results'; return; }
      grid.insertAdjacentHTML('beforeend', movies.map(card).join(''));
      status.textContent=q?`Search results for “${q}”`:'Popular movies • live from TMDB';
      if(movies.length===20) more?.classList.remove('hidden');
    }catch(e){ loading?.classList.add('hidden'); status.textContent='Movie catalogue could not be reached. Check your internet connection and TMDB key.'; if(reset) empty?.classList.remove('hidden'); }
  }
  search?.addEventListener('input',()=>{clearTimeout(timer);timer=setTimeout(()=>load(true),350);});
  more?.addEventListener('click',()=>{page++;load(false);});
  load(true);

  const city=document.getElementById('cityFilter'), movie=document.getElementById('movieFilter');
  function filterShows(){ document.querySelectorAll('#showGrid .show-card').forEach(x=>{const a=city?.value==='all'||x.dataset.city===city?.value;const b=movie?.value==='all'||x.dataset.movie===movie?.value;x.style.display=a&&b?'flex':'none';}); }
  city?.addEventListener('change',filterShows); movie?.addEventListener('change',filterShows);
})();

// Seat selection for booking page
(() => {
  const seats = document.getElementById('seats');
  if (!seats) return;
  const hidden = document.getElementById('selectedSeats');
  const summary = document.getElementById('summary');
  const total = document.getElementById('total');
  const seatType = document.getElementById('seatType');
  const booked = new Set(Array.isArray(window.booked) ? window.booked.map(Number) : []);
  const count = Number(seats.dataset.total || 60);
  const selected = new Set();

  function update() {
    const values = [...selected].sort((a,b) => a-b);
    if (hidden) hidden.value = values.join(',');
    if (summary) summary.textContent = `${values.length} seat${values.length === 1 ? '' : 's'}`;
    const rate = Number(window.rates?.[seatType?.value] ?? 0);
    if (total) total.textContent = `₹${(rate * values.length).toFixed(0)}`;
  }

  for (let i = 1; i <= count; i++) {
    const button = document.createElement('button');
    button.type = 'button';
    button.className = 'seat';
    button.textContent = i;
    button.title = booked.has(i) ? 'Booked' : `Seat ${i}`;
    if (booked.has(i)) {
      button.classList.add('booked');
      button.disabled = true;
    } else {
      button.addEventListener('click', () => {
        if (selected.has(i)) {
          selected.delete(i);
          button.classList.remove('selected');
        } else {
          if (selected.size >= 6) return;
          selected.add(i);
          button.classList.add('selected');
        }
        update();
      });
    }
    seats.appendChild(button);
  }
  seatType?.addEventListener('change', update);
  update();
})();
